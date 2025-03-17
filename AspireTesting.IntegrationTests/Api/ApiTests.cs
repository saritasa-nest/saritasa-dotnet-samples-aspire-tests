using AspireTesting.IntegrationTests.Abstractions;

namespace AspireTesting.IntegrationTests.Api
{
    [Trait("Category", "Integration")]
    public class ApiTests(ApiFixture fixture) : IClassFixture<ApiFixture>
    {
        [Fact]
        public async Task FirstNoteIsCorrect()
        {
            var note = await fixture.ApiClient.GetNoteByIdAsync(1);

            Assert.Equal("First Note", note.Text);
        }

        [Fact]
        public async Task CreateCorrectNote()
        {
            var note = new Note()
            {
                Text = "Second Note"
            };

            var createdNoteId = await fixture.ApiClient.CreateNoteAsync(note);

            var createdNote = await fixture.ApiClient.GetNoteByIdAsync(createdNoteId);

            Assert.Equal(note.Text, createdNote.Text);
        }
    }
}
