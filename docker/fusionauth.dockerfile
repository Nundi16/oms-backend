FROM fusionauth/fusionauth-app:1.64.0

ADD ./docker/fusionauth.kickstart.json /usr/local/fusionauth/kickstart/kickstart.json
ENV FUSIONAUTH_APP_KICKSTART_FILE=/usr/local/fusionauth/kickstart/kickstart.json
ENV FUSIONAUTH_APP_RUNTIME_MODE=production
