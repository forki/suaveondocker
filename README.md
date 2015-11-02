This is a proof of concept for running [Suave](http://suave.io/) on Docker.

# Create the docker image

    docker build -t forki/suave:0.1 .

# Run the container

    docker run -p 8084:8080 forki/suave:0.1

# Inspect the website

Go to 192.168.99.101:8084 (or whatever the ip of your hostsystem is).
