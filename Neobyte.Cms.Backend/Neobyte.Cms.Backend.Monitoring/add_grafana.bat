docker run -dit --name neobyte_cms_grafana_1 -e GF_SECURITY_ADMIN_USER=admin@neobyte.net -e GF_SECURITY_ADMIN_PASSWORD=Ne0byteCMS! -e GF_USERS_ALLOW_SIGN_UP=false -p 2900:3000 grafana/grafana:8.3.11