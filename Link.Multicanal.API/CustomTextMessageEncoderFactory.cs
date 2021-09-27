using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Text;

namespace Link.Multicanal.API
{
    public class CustomTextMessageEncoderFactory : MessageEncoderFactory
    {
        private CustomTextMessageEncoder encoder;
        private MessageVersion version;
        private string mediaType;
        private string charSet;

        internal CustomTextMessageEncoderFactory(string mediaType, string charSet,
            MessageVersion version)
        {
            this.version = version;
            this.mediaType = mediaType;
            this.charSet = charSet;
            this.encoder = new CustomTextMessageEncoder(this);

        }

        public CustomTextMessageEncoder CustomEncoder
        {
            get
            {
                return this.encoder;
            }
        }


        public override MessageEncoder Encoder
        {
            get
            {
                return this.encoder;
            }
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return this.version;
            }
        }

        internal string MediaType
        {
            get
            {
                return this.mediaType;
            }
        }

        internal string CharSet
        {
            get
            {
                return this.charSet;
            }
        }
    }
}
