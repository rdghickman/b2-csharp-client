﻿using NUnit.Framework;

using B2.Client.Rest;
using B2.Client.Rest.Request.Param;


namespace B2.Client.Test
{
    [TestFixture]
    public class UrlSegmentTest
    {
        [Test]
        [Category(TestCategories.Unit)]
        public void TestLiteralSegmentDoesNotTransform()
        {
            const string paramName = "paramName";
            const string paramValue = "paramValue";
            var param = new RestParam(paramName, paramValue);

            var segment = UrlSegment.Literal(paramName);

            Assert.That(segment.Transform(param.Yield()), Is.EqualTo(paramName));
        }

        [Test]
        [Category(TestCategories.Unit)]
        public void TestLiteralSegmentDoesNotThrow()
        {
            const string paramName = "paramName";
            const string paramValue = "paramValue";
            const string segmentName = "segmentName";
            var param = new RestParam(paramName, paramValue);

            var segment = UrlSegment.Literal(segmentName);

            //check that passing in parameters where none match is okay
            Assert.That(segment.Transform(param.Yield()), Is.EqualTo(segmentName));

            var conflictingParam = new RestParam(segmentName, paramValue);
            //check that multiple matching params are okay
            Assert.That(segment.Transform(conflictingParam.ConcatWith(conflictingParam)), Is.EqualTo(segmentName));
        }

        [Test]
        [Category(TestCategories.Unit)]
        public void TestParameterSegmentDoesTransform()
        {
            const string paramName = "paramName";
            const string paramValue = "paramValue";
            var param = new RestParam(paramName, paramValue);

            var segment = UrlSegment.Parameter(paramName);

            Assert.That(segment.Transform(param.Yield()), Is.EqualTo(paramValue));
        }

        [Test]
        [Category(TestCategories.Unit)]
        public void TestParameterSegmentWithNoMatchThrows()
        {
            const string paramName = "paramName";
            const string paramValue = "paramValue";
            const string segmentName = "segmentName";
            var param = new RestParam(paramName, paramValue);

            var segment = UrlSegment.Parameter(segmentName);

            Assert.That(() => segment.Transform(param.Yield()), Throws.InvalidOperationException);
        }

        [Test]
        [Category(TestCategories.Unit)]
        public void TestParameterSegmentWithMultipleMatchesThrows()
        {
            const string paramName = "paramName";
            const string paramValue = "paramValue";

            var param = new RestParam(paramName, paramValue);

            var segment = UrlSegment.Parameter(paramName);

            Assert.That(() => segment.Transform(param.ConcatWith(param)), Throws.InvalidOperationException);
        }
    }
}