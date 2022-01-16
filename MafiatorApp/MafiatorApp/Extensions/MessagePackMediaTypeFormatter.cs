using MessagePack;
using MessagePack.Resolvers;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MafiatorApp.Extensions
{
    public class MessagePackMediaTypeFormatter : MediaTypeFormatter
    {
        private static readonly MediaTypeHeaderValue ContentTypeMediaTypeHeader = MediaTypeHeaderValue.Parse(MessagePackHttpClientExtensions.ContentTypeString);

        public static readonly MessagePackMediaTypeFormatter DefaultInstance = new();

        public static readonly MediaTypeFormatter[] DefaultMediaTypeFormatters = { DefaultInstance };

        private static MediaTypeFormatterCollection _defaultMediaTypeFormatterCollection;

        public MessagePackSerializerOptions SerializerOptions { get;set; }

        public static MediaTypeFormatterCollection DefaultMediaTypeFormatterCollection
        {
            get
            {
                if (_defaultMediaTypeFormatterCollection != null) return _defaultMediaTypeFormatterCollection;
                _defaultMediaTypeFormatterCollection = new MediaTypeFormatterCollection();
                _defaultMediaTypeFormatterCollection.AddRange(DefaultMediaTypeFormatters);
                return _defaultMediaTypeFormatterCollection;
            }
        }

        public MessagePackMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(ContentTypeMediaTypeHeader);
            SerializerOptions = MessagePackSerializerOptions.Standard.WithResolver(StandardResolver.Instance);
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return CanReadType(type);
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            if (headers == null)
                throw new ArgumentNullException(nameof(headers));

            headers.ContentType = ContentTypeMediaTypeHeader;
        }

        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext)
        {
            await MessagePackSerializer.SerializeAsync(type, writeStream, value, SerializerOptions).ConfigureAwait(false);
        }

        public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext, CancellationToken cancellationToken)
        {
            await MessagePackSerializer.SerializeAsync(type, writeStream, value, SerializerOptions, cancellationToken).ConfigureAwait(false);
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger,
            CancellationToken cancellationToken)
        {
            return await MessagePackSerializer.DeserializeAsync(type, readStream, SerializerOptions, cancellationToken);
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return await MessagePackSerializer.DeserializeAsync(type, readStream, SerializerOptions);
        }
    }
}