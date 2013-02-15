﻿using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_by_value : DriverSpecs
    {
        [Test]
        public void Finds_radio_button_by_value() {
            Driver.FindField("radio field one val", Root).Name.should_be("forLabeledRadioFieldName");
            Driver.FindField("radio field two val", Root).Name.should_be("containerLabeledRadioFieldName");
        }

        [Test]
        public void Finds_checkbox_by_value() {
            Driver.FindField("checkbox one val", Root).Name.should_be("checkboxByValueOneFieldName");
            Driver.FindField("checkbox two val", Root).Name.should_be("checkboxByValueTwoFieldName");
        }

    }
}
