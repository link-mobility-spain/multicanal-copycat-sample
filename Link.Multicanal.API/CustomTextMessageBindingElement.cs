using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace Link.Multicanal.API
{
    public class CustomTextMessageBindingElement : MessageEncodingBindingElement
    {
        private MessageVersion msgVersion;
        private string mediaType;
        private string encoding;
        private XmlDictionaryReaderQuotas readerQuotas;
        private CustomTextMessageEncoderFactory customTextMessageEncoderFactory;
        
        CustomTextMessageBindingElement(CustomTextMessageBindingElement binding)
            : this(binding.customEncoderFactory)
        {
            this.readerQuotas = new XmlDictionaryReaderQuotas();
            binding.ReaderQuotas.CopyTo(this.readerQuotas);
        }

        public CustomTextMessageBindingElement(CustomTextMessageEncoderFactory customTextMessageEncoderFactory)
        {
            if (customTextMessageEncoderFactory == null)
                throw new ArgumentNullException(nameof(customTextMessageEncoderFactory));

            if (customTextMessageEncoderFactory.CharSet == null)
                throw new ArgumentNullException(nameof(customTextMessageEncoderFactory.CharSet));

            if (customTextMessageEncoderFactory.MediaType == null)
                throw new ArgumentNullException(nameof(customTextMessageEncoderFactory.MediaType));

            if (customTextMessageEncoderFactory.MessageVersion == null)
                throw new ArgumentNullException(nameof(customTextMessageEncoderFactory.MessageVersion));

            this.msgVersion = customTextMessageEncoderFactory.MessageVersion;
            this.mediaType = customTextMessageEncoderFactory.MediaType;
            this.encoding = customTextMessageEncoderFactory.CharSet;
            this.readerQuotas = new XmlDictionaryReaderQuotas();

            this.customTextMessageEncoderFactory = customTextMessageEncoderFactory;

        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return this.msgVersion;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                this.msgVersion = value;
            }
        }

        public string MediaType
        {
            get
            {
                return this.mediaType;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                this.mediaType = value;
            }
        }

        public string Encoding
        {
            get
            {
                return this.encoding;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                this.encoding = value;
            }
        }

        // This encoder does not enforces any quotas for the unsecure messages. The
        // quotas are enforced for the secure portions of messages when this encoder
        // is used in a binding that is configured with security.
        public XmlDictionaryReaderQuotas ReaderQuotas
        {
            get
            {
                return this.readerQuotas;
            }
        }

        public CustomTextMessageEncoderFactory customEncoderFactory
        {
            get
            {
                return this.customTextMessageEncoderFactory;
            }
        }

        #region IMessageEncodingBindingElement Members
        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return this.customTextMessageEncoderFactory;
        }

        #endregion

        public override BindingElement Clone()
        {
            return new CustomTextMessageBindingElement(this);
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelFactory<TChannel>();
        }

        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return context.CanBuildInnerChannelFactory<TChannel>();
        }

        public override T GetProperty<T>(BindingContext context)
        {
            if (typeof(T) == typeof(XmlDictionaryReaderQuotas))
            {
                return (T)(object)this.readerQuotas;
            }
            else
            {
                return base.GetProperty<T>(context);
            }
        }

    }
}
