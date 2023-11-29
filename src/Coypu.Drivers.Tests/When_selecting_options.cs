﻿using Coypu.Finders;
using Shouldly;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_selecting_options : DriverSpecs
    {
        private static DriverScope GetSelectScope(string locator)
        {
            var select = new BrowserWindow(DefaultSessionConfiguration,
                                         new SelectFinder(Driver, locator, Root, DefaultOptions), Driver,
                                         null, null, null, DisambiguationStrategy);
            return @select;
        }

        [Test]
        public void Sets_text_of_selected_option()
        {
            Field("containerLabeledSelectFieldId").SelectedOption.ShouldBe("select two option one");

            var option = FindSingle(new OptionFinder(Driver, "select two option two", GetSelectScope("containerLabeledSelectFieldId"), DefaultOptions));
            Driver.SelectOption(GetSelectScope("containerLabeledSelectFieldId").Now(), option, "select two option two");

            Field("containerLabeledSelectFieldId").SelectedOption.ShouldBe("select two option two");
        }

        [Test]
        public void Selected_option_respects_TextPrecision()
        {
            Assert.That(
                FindSingle(new OptionFinder(Driver, "select two option t", GetSelectScope("containerLabeledSelectFieldId"), Options.Substring)).Text,
                Is.EqualTo("select two option two"));

            Assert.That(
                FindSingle(new OptionFinder(Driver, "select two option two", GetSelectScope("containerLabeledSelectFieldId"), Options.Exact)).Text,
                Is.EqualTo("select two option two"));

            Assert.Throws<MissingHtmlException>(
                () => FindSingle(new OptionFinder(Driver, "select two option t", GetSelectScope("containerLabeledSelectFieldId"), Options.Exact)));
        }

        [Test]
        public void Selected_option_finds_exact_by_container_label()
        {
            Assert.That(FindSingle(new OptionFinder(Driver, "one", GetSelectScope("Ambiguous select options"), Options.Exact)).Text, Is.EqualTo("one"));
        }

        [Test]
        public void Selected_option_finds_substring_by_container_label()
        {
            Assert.That(FindSingle(new OptionFinder(Driver, "one", GetSelectScope("Ambiguous select options"), Options.Substring)).Text, Is.EqualTo("one"));
        }
    }

}
