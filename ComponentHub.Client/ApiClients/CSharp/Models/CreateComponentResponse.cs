// <auto-generated/>
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
namespace ComponentHub.ApiClients.Models {
    public class CreateComponentResponse : IParsable {
        /// <summary>The component property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public ComponentDto? Component { get; set; }
#nullable restore
#else
        public ComponentDto Component { get; set; }
#endif
        /// <summary>The page property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public ComponentPageDto? Page { get; set; }
#nullable restore
#else
        public ComponentPageDto Page { get; set; }
#endif
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static CreateComponentResponse CreateFromDiscriminatorValue(IParseNode parseNode) {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new CreateComponentResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers() {
            return new Dictionary<string, Action<IParseNode>> {
                {"component", n => { Component = n.GetObjectValue<ComponentDto>(ComponentDto.CreateFromDiscriminatorValue); } },
                {"page", n => { Page = n.GetObjectValue<ComponentPageDto>(ComponentPageDto.CreateFromDiscriminatorValue); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer) {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteObjectValue<ComponentDto>("component", Component);
            writer.WriteObjectValue<ComponentPageDto>("page", Page);
        }
    }
}
