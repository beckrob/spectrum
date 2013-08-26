using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.SpecSvc.Pipeline;

namespace Jhu.SpecSvc.Pipeline.Test
{
    [TestClass]
    public class PipelineSerializationTest
    {
        private void SerializePipelineTestHelper(SpectrumPipeline pipeline)
        {
            var sw = new StringWriter();
            var ser = new XmlSerializer(typeof(SpectrumPipeline));

            ser.Serialize(sw, pipeline);
        }

        [TestMethod]
        public void SerializePipelineTest()
        {
            var pipeline = new SpectrumPipeline();

            SerializePipelineTestHelper(pipeline);
        }

        [TestMethod]
        public void DeserializePipelineTest()
        {
            Assert.Inconclusive();
        }
    }
}
