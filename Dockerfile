FROM fsharp/fsharp
COPY ./src .
RUN mono ./.paket/paket.bootstrapper.exe
RUN mono ./.paket/paket.exe restore
CMD ["fsharpi", "helloworld.fsx"]