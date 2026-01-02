SHELL := bash

############# VARIABLES #############
CLIENT = apps/client
SERVER = apps/server
SERVER_SOLUTION  = apps/server/Server.sln
STARTUP_PROJECT  = apps/server/Server.API
INFRA_PROJECT    = apps/server/Server.Infrastructure
OPENAPI_JSON     = docs/openapi.json



############# build and start #############
.PHONY: build-server build-client \
		build start-server \
		start-client watch-server

build-server:
	dotnet build $(SERVER_SOLUTION)

build-client:
	cd $(CLIENT) && npm run build

build: build-server build-client

start-server:
	dotnet run --project apps/server/Server.API
watch-server:
	dotnet watch run --project apps/server/Server.API

start-client:
	cd $(CLIENT) && npm run dev



############## database ##############
.PHONY: add-migration update-database

add-migration:
ifeq ($(strip $(name)),)
	$(error ### Please provide a migration name. Example: make add-migration name=Init ###)
endif
	dotnet ef migrations add $(name) \
		--project $(INFRA_PROJECT) \
		--startup-project $(STARTUP_PROJECT)

update-database:
	dotnet ef database update \
		--project $(INFRA_PROJECT) \
		--startup-project $(STARTUP_PROJECT)



############## Linting and Formatting ##############
.PHONY: lint-check-client lint-fix-client lint-clients lint-fix-client format-fix-client \
		format-check-server format-fix-server \
		

format-check: lint-check-client format-check-client format-check-server
format-fix: lint-fix-client format-fix-client format-fix-server	

lint-check-client:
	cd $(CLIENT) && npx eslint . --ext .js,.jsx,.ts,.tsx

lint-fix-client:
	cd $(CLIENT) && npx eslint . --ext .js,.jsx,.ts,.tsx --fix

format-check-client:
	cd $(CLIENT) && npx prettier --check "**/*.{js,jsx,ts,tsx,json,css,md}"

format-fix-client:
	cd $(CLIENT) && npx prettier --write "**/*.{js,jsx,ts,tsx,json,css,md}"

format-check-server:
	cd $(SERVER) && dotnet format --verify-no-changes

format-fix-server:
	cd $(SERVER) && dotnet format ./Server.sln



############## Install dependencies for local setup ############+##
.PHONY: install

install:
	cd $(CLIENT) && npm install
	dotnet restore $(SERVER_SOLUTION)

	

############## OpenAPI and type generation ##############
.PHONY: export-openapi generate-ts-types sync-api-contracts

export-openapi:
	dotnet tool run swagger tofile --output ./$(OPENAPI_JSON) ./apps/Server/Server.API/bin/Debug/net8.0/Server.API.dll v1

generate-ts-types:
	cd $(CLIENT) && npx openapi-typescript ../../$(OPENAPI_JSON) --output src/types/generated/api.d.ts

sync-api-contracts: build-server export-openapi generate-ts-types