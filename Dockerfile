FROM mono:5.12
RUN mkdir -p /sharpchat
WORKDIR /sharpchat
COPY . /sharpchat
EXPOSE 1986
RUN apt update && apt install -y gtk3.0
CMD ["mono", "./SharpChat/bin/Debug/SharpChat.exe" ]
