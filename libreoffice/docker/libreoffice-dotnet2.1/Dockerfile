FROM microsoft/dotnet:2.1-aspnetcore-runtime


FROM sersms/libreoffice

# dotnet runtime拷贝到容器中
COPY  --from=0 /usr/share/dotnet /usr/share/dotnet 

ENV ASPNETCORE_VERSION=2.1.24
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

