using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DbChart;

public static class MermaidDiagramGenerator
{
    public static string Generate(IModel databaseModel, ITable table)
    {
        var tableMappings = databaseModel
            .GetEntityTypes()
            .SelectMany(x => x.GetTableMappings())
            .ToList();
        
        var sb = new StringBuilder("erDiagram\n");
        
        var includedNavigations = new HashSet<IReadOnlyNavigation>();
        
        var entityType = table.EntityTypeMappings.First().TypeBase.ContainingEntityType;
        
        sb.Append($"    {table.Name} {{");
        foreach (var col in table.Columns)
        {
            sb.AppendLine($"string {col.Name}");
        }
        sb.AppendLine("}");

        foreach (IReadOnlyNavigation navigation in entityType.GetNavigations())
        {
            if (!includedNavigations.Add(navigation) || 
                (navigation.Inverse is { } inverse && !includedNavigations.Add(inverse)))
                continue;
                
            var forwardDir = navigation.IsCollection;
            var reverseDir = navigation.Inverse?.IsCollection ?? false;
                
            sb.Append($"    {table.Name}");
            sb.Append(reverseDir ? " }|" : " ||");
            sb.Append("--");
            sb.Append(forwardDir ? "|{ " : "|| ");
            sb.Append($"\"{navigation.TargetEntityType.GetTableName()}\"");
            sb.Append($" : \"{GetRelationshipDescription(navigation)}\"");
            sb.AppendLine();

            sb.Append($"    {navigation.TargetEntityType.GetTableName()} {{");
            foreach (var col in tableMappings.Where(t => t.Table.Name == navigation.TargetEntityType.GetTableName()).SelectMany(x => x.ColumnMappings))
            {
                sb.AppendLine($"string {col.Column.Name}");
            }
            sb.AppendLine("}");
        }

        return sb.ToString();
    }
    
    public static string Generate(IModel databaseModel)
    {
        var sb = new StringBuilder("erDiagram\n");
        
        var tables = databaseModel
            .GetEntityTypes()
            .SelectMany(x => x.GetTableMappings())
            .Select(x => x.Table);

        var includedNavigations = new HashSet<IReadOnlyNavigation>();
        
        foreach (var table in tables)
        {
            var entityType = table.EntityTypeMappings.First().TypeBase.ContainingEntityType;
            foreach (IReadOnlyNavigation navigation in entityType.GetNavigations())
            {
                if (!includedNavigations.Add(navigation) || 
                    (navigation.Inverse is { } inverse && !includedNavigations.Add(inverse)))
                    continue;
                
                var forwardDir = navigation.IsCollection;
                var reverseDir = navigation.Inverse?.IsCollection ?? false;
                
                sb.Append($"    {table.Name}");
                sb.Append(reverseDir ? " }|" : " ||");
                sb.Append("--");
                sb.Append(forwardDir ? "|{ " : "|| ");
                sb.Append($"\"{navigation.TargetEntityType.GetTableName()}\"");
                sb.Append($" : \"{GetRelationshipDescription(navigation)}\"");
                
                sb.AppendLine();
            }

            sb.Append($"    {table.Name} {{");
            foreach (var col in table.Columns)
            {
                sb.AppendLine($"string {col.Name}");
            }
            sb.AppendLine("}");
        }

        return sb.ToString();
    }
    
    private static string? GetRelationshipDescription(IReadOnlyNavigation navigation) => navigation
        .PropertyInfo?
        .GetCustomAttributes(typeof(RelationshipDescriptionAttribute), false)
        .Cast<RelationshipDescriptionAttribute>()
        .FirstOrDefault()?
        .Description;
}