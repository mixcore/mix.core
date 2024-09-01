using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using Mix.Communicator.Models;

namespace Mix.Communicator.Services
{
    // Ref: https://firebase.google.com/docs/cloud-messaging/send-message
    public class FirestoreService
    {
        private readonly FirestoreDb _db;
        private readonly FirebaseSettingModel _settings = new FirebaseSettingModel();
        public FirestoreService(IConfiguration configuration)
        {
            configuration.GetSection(MixAppSettingsSection.GoogleFirebase).Bind(_settings);
            if (!string.IsNullOrEmpty(_settings.ProjectId) && !string.IsNullOrEmpty(_settings.Filename))
            {

                var googleCredential = _settings.Filename;

                var credential = GoogleCredential.FromFile(googleCredential);
                var builder = new FirestoreDbBuilder
                {
                    ProjectId = _settings.ProjectId,
                    Credential = credential,
                };
                _db = builder.Build();
            }
        }

        public async Task<DocumentReference> GetDocumentAsync(string collectionPath, string documentId)
        {
            return _db.Collection(collectionPath).Document(documentId);
        }

        public async Task<WriteResult> AddDocumentAsync(string collectionPath, Dictionary<string, object> data)
        {
            DocumentReference docRef = _db.Collection(collectionPath).Document();
            return await docRef.SetAsync(data);
        }

        public async Task<WriteResult> UpdateDocumentAsync(string collectionPath, string documentId, Dictionary<string, object> data)
        {
            DocumentReference docRef = _db.Collection(collectionPath).Document(documentId);
            return await docRef.UpdateAsync(data);
        }

        public async Task DeleteDocumentAsync(string collectionPath, string documentId)
        {
            DocumentReference docRef = _db.Collection(collectionPath).Document(documentId);
            await docRef.DeleteAsync();
        }


    }
}
