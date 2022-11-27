using System;
using System.Collections.Generic;
using System.Linq;

namespace Mafiator.IocConfig.Formatters
{
    public class MessagePackAttributesSchemaProcessor //: ISchemaProcessor
    {
        //public void Process(SchemaProcessorContext context)
        //{
        //    if (context == null)
        //        throw new ArgumentNullException(nameof(context));

        //    var msgPackAttr = context.Type.GetCustomAttributes(true).OfType<MessagePack.MessagePackObjectAttribute>()
        //        .SingleOrDefault();
        //    if (msgPackAttr == null) return;
        //    context.Schema.ExtensionData ??= new Dictionary<string, object>();

        //    context.Schema.ExtensionData.Add("x-msgpack", true);

        //    // loop through types
        //    foreach (var properties in context.Type.GetProperties())
        //    {
        //        var name = context.Generator.GetPropertyName(null, properties.ToContextualAccessor());

        //        var (_, schemaProp) = context.Schema.Properties
        //            .FirstOrDefault(p => p.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        //        if (schemaProp == null)
        //            continue;

        //        var keyAttr = properties.GetCustomAttributes(true).OfType<MessagePack.KeyAttribute>().SingleOrDefault();
        //        if (keyAttr != null)
        //        {
        //            schemaProp.ExtensionData ??= new Dictionary<string, object>();
        //            schemaProp.ExtensionData.Add("x-msgpack-key", keyAttr.IntKey);
        //        }

        //        var ignoreAttr = properties.GetCustomAttributes(true).OfType<MessagePack.IgnoreMemberAttribute>()
        //            .SingleOrDefault();
        //        if (ignoreAttr == null) continue;
        //        schemaProp.ExtensionData ??= new Dictionary<string, object>();
        //        schemaProp.ExtensionData.Add("x-msgpack-ignore", true);
        //    }
        //}
    }
}