FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
WORKDIR "/src/Neobyte.Cms.Backend/Neobyte.Cms.Backend"
RUN dotnet restore "Neobyte.Cms.Backend.csproj"
RUN dotnet build "Neobyte.Cms.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Neobyte.Cms.Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /puppeteer

# Begin Puppeteer setup
RUN apt-get update
RUN apt-get install -qq fonts-liberation gconf-service libappindicator1 libasound2 libatk1.0-0 libcairo2 libcups2 libfontconfig1 libgbm-dev libgdk-pixbuf2.0-0 libgtk-3-0 libicu-dev libjpeg-dev libnspr4 libnss3 libpango-1.0-0 libpangocairo-1.0-0 libpng-dev libx11-6 libx11-xcb1 libxcb1 libxcomposite1 libxcursor1 libxdamage1 libxext6 libxfixes3 libxi6 libxrandr2 libxrender1 libxss1 libxtst6 xdg-utils

RUN apt-get -y install wget gnupg2 apt-utils
RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add - \
    && sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list' \
    && apt-get update \
    && apt-get install -y google-chrome-unstable fonts-ipafont-gothic fonts-wqy-zenhei fonts-thai-tlwg fonts-kacst fonts-freefont-ttf \
      --no-install-recommends \
    && rm -rf /var/lib/apt/lists/*

	
ENV PUPPETEER_EXECUTABLE_PATH "/usr/bin/google-chrome-unstable"
# End Puppeteer setup
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Neobyte.Cms.Backend.dll"]