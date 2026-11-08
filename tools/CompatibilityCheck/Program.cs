using System.Reflection;
using System.Text.Json;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: CompatibilityCheck <game directory> <AmbientLights.dll>");
    return 1;
}

// Inspect metadata only: loading IL2CPP wrappers for execution requires a running Unity player.
var gamePath = Path.GetFullPath(args[0]);
var modPath = Path.GetFullPath(args[1]);
var assemblies = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
foreach (var directory in new[] {
    Path.GetDirectoryName(typeof(object).Assembly.Location)!,
    Path.Combine(gamePath, "MelonLoader", "net6"),
    Path.Combine(gamePath, "MelonLoader", "Il2CppAssemblies"),
    Path.Combine(gamePath, "Mods") })
{
    foreach (var path in Directory.EnumerateFiles(directory, "*.dll"))
        assemblies[Path.GetFileName(path)] = path;
}
assemblies[Path.GetFileName(modPath)] = modPath;
using var context = new MetadataLoadContext(new PathAssemblyResolver(assemblies.Values), "System.Private.CoreLib");
var mod = context.LoadFromAssemblyPath(modPath);
int errors = 0, count = 0;
const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
foreach (var type in mod.GetTypes())
{
    foreach (var patch in type.GetCustomAttributesData().Where(a => a.AttributeType.FullName == "HarmonyLib.HarmonyPatch"))
    {
        var values = patch.ConstructorArguments;
        if (values.Count < 2 || values[0].Value is not Type targetType || values[1].Value is not string methodName)
            continue;
        count++;
        Type[]? argumentTypes = values.Count > 2 && values[2].Value is IList<CustomAttributeTypedArgument> types
            ? types.Select(t => (Type)t.Value!).ToArray() : null;
        var targets = targetType.GetMethods(flags).Where(m => m.Name == methodName &&
            (argumentTypes == null || m.GetParameters().Select(p => p.ParameterType).SequenceEqual(argumentTypes))).ToArray();
        if (targets.Length != 1)
        {
            Fail($"{type.Name}: expected one target for {targetType.Name}.{methodName}, found {targets.Length}.");
            continue;
        }
        var target = targets[0];
        foreach (var hook in type.GetMethods(flags).Where(m => m.Name is "Prefix" or "Postfix"))
        {
            foreach (var parameter in hook.GetParameters())
            {
                if (parameter.Name == "__instance")
                {
                    if (target.IsStatic || !parameter.ParameterType.IsAssignableFrom(targetType))
                        Fail($"{type.Name}: invalid __instance ({parameter.ParameterType.Name}) on {target}.");
                }
                else if (parameter.Name?.StartsWith("__") != true)
                {
                    var original = target.GetParameters().SingleOrDefault(p => p.Name == parameter.Name);
                    var parameterType = parameter.ParameterType.IsByRef ? parameter.ParameterType.GetElementType()! : parameter.ParameterType;
                    if (original == null || original.ParameterType != parameterType)
                        Fail($"{type.Name}: parameter {parameter.Name} does not match {target}.");
                }
            }
        }
        Console.WriteLine($"Checked {targetType.Name}.{target.Name}");
    }
}
Console.WriteLine($"Checked {count} Harmony patches; {errors} errors.");
var configDirectory = Path.Combine(Path.GetDirectoryName(modPath)!, "ambient-lights");
int configs = 0;
foreach (var file in Directory.EnumerateFiles(configDirectory, "*.json"))
{
    try
    {
        using var json = JsonDocument.Parse(File.ReadAllText(file), new JsonDocumentOptions
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip
        });
        configs++;
    }
    catch (JsonException exception)
    {
        Fail($"{Path.GetFileName(file)}: {exception.Message}");
    }
}
Console.WriteLine($"Parsed {configs} shipped JSON files; {errors} total errors.");
return errors == 0 && count > 0 ? 0 : 1;

void Fail(string message)
{
    Console.Error.WriteLine(message);
    errors++;
}
