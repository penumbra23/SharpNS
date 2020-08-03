
# SharpNS

 DNS server with RESTful API for managing records.

## Intro

**SharpNS** implements the  DNS server specification and exposes it on port 53. For managing DNS record, it has multiple implementations:
- _in-memory store_
- _SQLite file database_
- _UDP proxy_ (specifying the desired public DNS server)

The RESTful API exposes one resource: `/record`, with endpoints to list, retrieve one record, create new ones, edit and delete existing records.

## Usage

Simply clone the repo and run the *.sln* file inside Visual Studio. For testing the DNS server, use any client that comforts you (my advice is running it inside a container, easiest one to setup and test).

For running inside Docker or Kubernetes, you can build and the image yourself:
```bash
cd SharpNS
docker build -t dns-image .
docker run -p 80:80 dns-image
```

or use the public one from [DockerHub](https://hub.docker.com/r/penumbra23/sharpns).

## Docker Compose example

For running the example given by the `docker-compose` file, **first** make sure to delete the existing network, by simply running:

`docker-compose down`

After that run the example with:

`docker-compose up`

The `cli-tester` services uses the `sharpns` image as the desired DNS server. The CLI should printout the queried DNS records, both from the service and Google's public DNS server **8.8.8.8**.

## License
MIT