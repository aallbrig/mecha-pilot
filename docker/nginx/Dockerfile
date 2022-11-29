FROM nginx:alpine

WORKDIR /etc/nginx/conf.d
COPY ./etc/nginx/conf.d/default.conf default.conf

WORKDIR /webgl
COPY ./webgl .
