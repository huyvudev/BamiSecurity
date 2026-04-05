FROM node:18
WORKDIR /app

COPY package.json /app/
RUN yarn install
RUN yarn add sass

COPY . /app/
ARG PROJECT
ARG CONFIGURATION
RUN node_modules/.bin/sass projects/${PROJECT}/src/assets
RUN node_modules/.bin/ng build ${PROJECT} --configuration=${CONFIGURATION}

FROM nginx:alpine
ARG PROJECT
RUN echo OK > /usr/share/nginx/html/health
COPY --from=0 /app/dist/${PROJECT}/ /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
