using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;
using System.IO;

namespace Link.Multicanal.API
{
    public class CustomTextMessageEncoder : MessageEncoder
    {
        private CustomTextMessageEncoderFactory factory;
        private XmlWriterSettings writerSettings;
        private string contentType;

        public event EventHandler<MessageInspectorArgs> OnMessageInspected;

        public CustomTextMessageEncoder(CustomTextMessageEncoderFactory factory)
        {
            this.factory = factory;

            this.writerSettings = new XmlWriterSettings();
            this.writerSettings.Encoding = Encoding.GetEncoding(factory.CharSet);
            this.contentType = $"{this.factory.MediaType};charset={Encoding.UTF8.HeaderName}";
            
        }

        public override string ContentType
        {
            get
            {
                return this.contentType;
            }
        }

        public override string MediaType
        {
            get
            {
                return factory.MediaType;
            }
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return this.factory.MessageVersion;
            }
        }


        public override bool IsContentTypeSupported(string contentType)
        {
            return true;
        }

        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            byte[] msgContents = new byte[buffer.Count];
            Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);
            bufferManager.ReturnBuffer(buffer.Array);

            MemoryStream stream = new MemoryStream(msgContents);
            return ReadMessage(stream, int.MaxValue);
        }

        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
        {
            XmlReader reader = XmlReader.Create(stream);
            return Message.CreateMessage(reader, maxSizeOfHeaders, this.MessageVersion);
        }

        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            MemoryStream stream = new MemoryStream();
            // Force XML Body encoding to UTF-8
            this.writerSettings.Encoding = Encoding.UTF8;
            XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);
            message.WriteMessage(writer);
            writer.Close();

            // Change XML encoding header to factory.CharSet (iso-8859-1)
            var xml = message.ToString();
            xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", $"<?xml version=\"1.0\" encoding=\"{Encoding.GetEncoding(factory.CharSet).HeaderName}\"?>");
            xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", $"<?xml version=\"1.0\" encoding=\"{Encoding.GetEncoding(factory.CharSet).HeaderName}\"?>");

            //byte[] messageBytes = stream.GetBuffer();
            byte[] messageBytes = this.writerSettings.Encoding.GetBytes(xml);

            if (OnMessageInspected != null)
            {
                // Notify the subscribers of the inpected message.
                OnMessageInspected(this, new MessageInspectorArgs { Message = this.writerSettings.Encoding.GetString(messageBytes), MessageInspectionType = eMessageInspectionType.Request });
            }
            
            //int messageLength = (int)stream.Position;
            int messageLength = (int)messageBytes.Length;

            stream.Close();

            int totalLength = messageLength + messageOffset;
            byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
            Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

            ArraySegment<byte> byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
            return byteArray;
        }

        public override void WriteMessage(Message message, Stream stream)
        {
            XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);
            message.WriteMessage(writer);
            writer.Close();
        }
    }

}
