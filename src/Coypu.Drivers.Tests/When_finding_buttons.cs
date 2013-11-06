using NSpec;
using NUnit.Framework;
namespace Coypu.Drivers.Tests
{
    public class When_finding_buttons : DriverSpecs
    {
        [Test]
        public void Finds_a_particular_button_by_its_text()
        {
            Driver.FindButton("first button", Root).Id.should_be("firstButtonId");
            Driver.FindButton("second button", Root).Id.should_be("secondButtonId");
        }

        [Test]
        public void Finds_a_particular_button_by_its_id()
        {
            Driver.FindButton("firstButtonId", Root).Text.should_be("first button");
            Driver.FindButton("thirdButtonId", Root).Text.should_be("third button");
        }

        [Test]
        public void Finds_a_particular_button_by_its_name()
        {
            Driver.FindButton("secondButtonName", Root).Text.should_be("second button");
            Driver.FindButton("thirdButtonName", Root).Text.should_be("third button");
        }

        [Test]
        public void Finds_a_particular_input_button_by_its_value()
        {
            Driver.FindButton("first input button", Root).Id.should_be("firstInputButtonId");
            Driver.FindButton("second input button", Root).Id.should_be("secondInputButtonId");
        }

        [Test]
        public void Finds_a_particular_input_button_by_its_id()
        {
            Driver.FindButton("firstInputButtonId", Root).Value.should_be("first input button");
            Driver.FindButton("thirdInputButtonId", Root).Value.should_be("third input button");
        }

        [Test]
        public void Finds_a_particular_input_button_by_its_name()
        {
            Driver.FindButton("secondInputButtonId", Root).Value.should_be("second input button");
            Driver.FindButton("thirdInputButtonName", Root).Value.should_be("third input button");
        }

        [Test]
        public void Finds_a_particular_submit_button_by_its_value()
        {
            Driver.FindButton("first submit button", Root).Id.should_be("firstSubmitButtonId");
            Driver.FindButton("second submit button", Root).Id.should_be("secondSubmitButtonId");
        }

        [Test]
        public void Finds_a_particular_submit_button_by_its_id()
        {
            Driver.FindButton("firstSubmitButtonId", Root).Value.should_be("first submit button");
            Driver.FindButton("thirdSubmitButtonId", Root).Value.should_be("third submit button");
        }

        [Test]
        public void Finds_a_particular_submit_button_by_its_name()
        {
            Driver.FindButton("secondSubmitButtonName", Root).Value.should_be("second submit button");
            Driver.FindButton("thirdSubmitButtonName", Root).Value.should_be("third submit button");
        }

        [Test]
        public void Finds_image_buttons()
        {
            Driver.FindButton("firstImageButtonId", Root).Value.should_be("first image button");
            Driver.FindButton("secondImageButtonId", Root).Value.should_be("second image button");
        }

        [Test]
        public void Does_not_find_text_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Driver.FindButton("firstTextInputId", Root));
        }

        [Test]
        public void Does_not_find_email_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Driver.FindButton("firstEmailInputId", Root));
        }

        [Test]
        public void Does_not_find_tel_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Driver.FindButton("firstTelInputId", Root));
        }

        [Test]
        public void Does_not_find_url_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Driver.FindButton("firstUrlInputId", Root));
        }

        [Test]
        public void Does_not_find_hidden_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Driver.FindButton("firstHiddenInputId", Root));
        }

        [Test]
        public void Does_not_find_invisible_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Driver.FindButton("firstInvisibleInputId", Root));
        }

        [Test]
        public void Finds_img_elements_with_role_button_by_alt_text()
        {
            Assert.That(Driver.FindButton("I'm an image with the role of button", Root).Id, Is.EqualTo("roleImageButtonId"));
        }

        [Test]
        public void Finds_any_elements_with_role_button_by_text()
        {
            Assert.That(Driver.FindButton("I'm a span with the role of button", Root).Id, Is.EqualTo("roleSpanButtonId"));
        }

        [Test]
        public void Finds_any_elements_with_class_button_by_text()
        {
            Assert.That(Driver.FindButton("I'm a span with the class of button", Root).Id, Is.EqualTo("classButtonSpanButtonId"));
        }

        [Test]
        public void Finds_any_elements_with_class_btn_by_text()
        {
            Assert.That(Driver.FindButton("I'm a span with the class of btn", Root).Id, Is.EqualTo("classBtnSpanButtonId"));
        }

        [Test]
        public void Finds_an_image_button_with_both_types_of_quote_in_my_value()
        {
            var button = Driver.FindButton("I'm an image button with \"both\" types of quote in my value", Root);
            Assert.That(button.Id, Is.EqualTo("buttonWithBothQuotesId"));
        }
    }
}