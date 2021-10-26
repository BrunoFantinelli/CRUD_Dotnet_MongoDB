using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBBI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Conecta no Banco de Dados
            var settings = MongoClientSettings.FromConnectionString("mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&directConnection=true&ssl=false");
            
            // Cria um novo Client com as configurações
            var client = new MongoClient(settings);

            // Pega o database
            var database = client.GetDatabase("local");

            // Pega a coleção
            var collection = database.GetCollection<BsonDocument>("BI");


            //Cria documento
            var document = new BsonDocument
                                        {
                                            { "Nome", "Bruno" },
                                            { "Sobrenome", "Fantineli" },
                                            { "Telefone", new BsonDocument
                                                {
                                                    { "Celular", 91569117 },
                                                    { "TelefoneCasa", 35323572 }
                                                }}
                                        };

            //Insere na coleção
            collection.InsertOne(document);

            //Documento Async
            var documentAsync = new BsonDocument
            {
                {"Nome", "Luana" },
                {"Sobrenome", "Pereira" },
                {"Email", "reispereiraluana@gmail.com" }
            };

            //Insere na coleção
            await collection.InsertOneAsync(documentAsync);

            //Insere vários documentos de uma vez
            var contador = Enumerable.Range(0, 10).Select(i => new BsonDocument("contador", i));
            await collection.InsertManyAsync(contador);

            //Conta quantos documentos tem na coleção
            var count = collection.CountDocuments(new BsonDocument());

            // Busca primeiro documento documento
            var documentBusca = collection.Find(new BsonDocument()).FirstOrDefault();

            // Busca todos os documentos
            var documents = collection.Find(new BsonDocument()).ToList();

            //Filtra documento busca por nome
            var filter = Builders<BsonDocument>.Filter.Eq("Nome", "Bruno");
            var buscaPorNome = collection.Find(filter).First();

            // Pega os números 
            var filterBuilder = Builders<BsonDocument>.Filter;
            var filterNumeros = filterBuilder.Gt("contador", 4) & filterBuilder.Lte("contador", 10);
            var cursor = collection.Find(filterNumeros).ToList();

            //Atualizar os dados
            var filterName = Builders<BsonDocument>.Filter.Eq("Nome", "Bruno");
            var update = Builders<BsonDocument>.Update.Set("Nome", "Amanda");
            collection.UpdateOne(filterName, update);

            //Deletar o documento
            var filterDelete = Builders<BsonDocument>.Filter.Eq("Nome", "Amanda");
            collection.DeleteOne(filterDelete);
        }
    }
}
    