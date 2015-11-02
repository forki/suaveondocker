FROM fsharp/fsharp
COPY ./.paket/paket.bootstrapper.exe ./.paket/paket.bootstrapper.exe
COPY ./src .
RUN mono ./.paket/paket.bootstrapper.exe
RUN mono ./.paket/paket.exe restore
CMD ["fsharpi", "helloworld.fsx"]