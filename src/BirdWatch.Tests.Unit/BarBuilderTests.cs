using System;
using UnityEngine;
using Xunit;
using Object = UnityEngine.Object;

namespace BirdWatch.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class BarBuilderTests
    {
        [Theory]
        [InlineData(0f)]
        [InlineData(.3f)]
        [InlineData(.5f)]
        [InlineData(.9f)]
        [InlineData(1f)]
        public void CalculateStartPoint_GivesScale_GivenOneScale(float ratio)
        {
            var input = Vector3.zero;
            var actual = CalculateStartPoint(input, 1, ratio);
            Assert.Equal(ratio, actual.x);
        }

        [Theory]
        [InlineData(1f, .5f)]
        [InlineData(2f, 1f)]
        [InlineData(4f, 2f)]
        public void CalculateStartPoint_GivesExpected_GivenXscale(float scale, float expected)
        {
            var input = Vector3.zero;
            var actual = CalculateStartPoint(input, scale, .5f);
            Assert.Equal(expected, actual.x);
        }


        [Theory]
        [InlineData(1f, 1.5f)]
        [InlineData(2f, 2f)]
        [InlineData(4f, 3f)]
        public void CalculateStartPoint_GivesExpected_GivenVectorValue(float scale, float expected)
        {
            var input = Vector3.one;
            var actual = CalculateStartPoint(input, scale, .5f);
            Assert.Equal(expected, actual.x);
        }


        [Theory]
        [InlineData(1f, .5f)]
        [InlineData(2f, 1f)]
        [InlineData(4f, 2f)]
        public void CalculateScale_GivesExpected_GivenVectorValue(float scale, float expected)
        {
            var input = Vector3.one;
            var actual = CalculateScale(scale, .5f);
            Assert.Equal(expected, actual.x);
        }


        private Vector3 CalculateStartPoint(Vector3 origin, float xScale, float ratio)
        {
            return origin + Vector3.one * ratio * xScale;
        }

        private Vector3 CalculateScale(float xScale, float ratio)
        {
            return Vector3.one * xScale * ratio;
        }

    }
}
