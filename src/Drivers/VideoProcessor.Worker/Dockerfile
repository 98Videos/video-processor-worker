FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

# Download FFMPEG Binaries
FROM alpine AS ffmpeg-files
RUN apk update
RUN apk add curl
RUN apk add xz
RUN apk add tar

RUN curl -L -O https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-linux64-gpl.tar.xz
RUN tar -xf ffmpeg-master-latest-linux64-gpl.tar.xz
RUN mkdir binaries
RUN cp ./ffmpeg-master-latest-linux64-gpl/bin/* ./binaries/

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Drivers/VideoProcessor.Worker/VideoProcessor.Worker.csproj", "Drivers/VideoProcessor.Worker/"]
COPY ["Core/VideoProcessor.Application/VideoProcessor.Application.csproj", "Core/VideoProcessor.Application/"]
COPY ["Core/VideoProcessor.Domain/VideoProcessor.Domain.csproj", "Core/VideoProcessor.Domain/"]
COPY ["Drivens/VideoProcessor.Clients.VideoManager/VideoProcessor.Clients.VideoManager.csproj", "Drivens/VideoProcessor.Clients.VideoManager/"]
COPY ["Drivens/VideoProcessor.Data.S3/VideoProcessor.Data.S3.csproj", "Drivens/VideoProcessor.Data.S3/"]
COPY ["Drivens/VideoProcessor.FFMPEG/VideoProcessor.FFMPEG.csproj", "Drivens/VideoProcessor.FFMPEG/"]
RUN dotnet restore "./Drivers/VideoProcessor.Worker/VideoProcessor.Worker.csproj"

COPY . .

WORKDIR "/src/Drivers/VideoProcessor.Worker"
RUN dotnet build "./VideoProcessor.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VideoProcessor.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=ffmpeg-files /binaries ./LibraryBinaries
ENTRYPOINT ["dotnet", "VideoProcessor.Worker.dll"]