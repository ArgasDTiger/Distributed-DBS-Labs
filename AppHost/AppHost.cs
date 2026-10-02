using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgres")
    .WithDataVolume("distributed-db-postgres-data")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEndpoint(port: 5433, name: "tcp");

var postgresDb = postgres.AddDatabase("booksdb");

var kafka = builder
    .AddKafka("kafka")
    .WithDataVolume("distributed-db-kafka-data")
    .WithKafkaUI();

var mongo = builder
    .AddMongoDB("mongo")
    .WithDataVolume("distributed-db-mongo-data")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEndpoint(port: 27017)
    .WithMongoExpress();

var mongoDb = mongo.AddDatabase("mongo-booksdb");

var migrations = builder.AddProject<MigrationService>("migrations")
    .WithReference(postgresDb, connectionName: "Postgres")
    .WaitFor(postgres);

builder.AddProject<Producer>("producer")
    .WithReference(postgresDb, connectionName: "Postgres")
    .WithReference(kafka)
    .WaitForCompletion(migrations)
    .WaitFor(kafka);

builder.AddProject<Consumer>("consumer")
    .WithReference(kafka)
    .WithReference(mongoDb)
    .WaitFor(kafka)
    .WaitFor(mongoDb);

builder.Build().Run();