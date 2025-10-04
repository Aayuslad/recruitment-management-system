SHELL := bash

############# VARIABLES #############
CANDIDATE_CLIENT = apps/candidate-client
RECRUITER_CLIENT = apps/recruiter-client
SERVER_SOLUTION  = apps/server/Server.sln
STARTUP_PROJECT  = apps/server/Server.API
INFRA_PROJECT    = apps/server/Server.Infrastructure
INFRA_MIGRATIONS_DIR = ../../../infra/db/migrations
OPENAPI_JSON     = docs/openapi.json



############# build and start #############
.PHONY: build-server build-recruiter-client build-candidate-client \
		build-clients build-all start-server start-candidate-client \
		start-recruiter-client start-all watch-server

build-server:
	dotnet build $(SERVER_SOLUTION)

build-recruiter-client:
	cd $(RECRUITER_CLIENT) && npm run build

build-candidate-client:
	cd $(CANDIDATE_CLIENT) && npm run build

build-clients: build-recruiter-client build-candidate-client

build-all: build-server build-clients

start-server:
	dotnet run --project apps/server/Server.API
watch-server:
	dotnet watch run --project apps/server/Server.API

start-candidate-client:
	cd $(CANDIDATE_CLIENT) && npm run dev

start-recruiter-client:
	cd $(RECRUITER_CLIENT) && npm run dev

start-all: start-server start-candidate-client start-recruiter-client




############## database ##############
.PHONY: add-migration update-database seed-database

add-migration:
ifeq ($(strip $(name)),)
	$(error ❌ Please provide a migration name. Example: make add-migration name=Init)
endif
	dotnet tool run dotnet-ef migrations add $(name) \
		--project $(INFRA_PROJECT) \
		--startup-project $(STARTUP_PROJECT) \
		--output-dir $(INFRA_MIGRATIONS_DIR)

update-database:
ifeq ($(strip $(name)),)
	dotnet tool run dotnet-ef database update \
		--project $(INFRA_PROJECT) \
		--startup-project $(STARTUP_PROJECT)
else
	dotnet tool run dotnet-ef database update $(name) \
		--project $(INFRA_PROJECT) \
		--startup-project $(STARTUP_PROJECT)
endif

seed-database:
	dotnet run --project infra/db/seeding



############## Linting and Formatting ##############
.PHONY: lint-candidate lint-recruiter lint-clients lint-fix-candidate \
		lint-fix-recruiter format-check-candidate format-check-recruiter \
		format-check-server format-server format-clients format-fix-candidate \
		format-fix-recruiter format-fix-clients  check fix

check: lint-clients format-clients format-server
fix: lint-fix-candidate lint-fix-recruiter format-fix-candidate format-fix-recruiter format-server	

lint-candidate:
	cd $(CANDIDATE_CLIENT) && npx eslint . --ext .js,.jsx,.ts,.tsx

lint-recruiter:
	cd $(RECRUITER_CLIENT) && npx eslint . --ext .js,.jsx,.ts,.tsx

lint-clients: lint-candidate lint-recruiter

lint-fix-candidate:
	cd $(CANDIDATE_CLIENT) && npx eslint . --ext .js,.jsx,.ts,.tsx --fix

lint-fix-recruiter:
	cd $(RECRUITER_CLIENT) && npx eslint . --ext .js,.jsx,.ts,.tsx --fix

format-check-candidate:
	cd $(CANDIDATE_CLIENT) && npx prettier --check "**/*.{js,jsx,ts,tsx,json,css,md}"

format-check-recruiter:
	cd $(RECRUITER_CLIENT) && npx prettier --check "**/*.{js,jsx,ts,tsx,json,css,md}"

format-check-server:
	dotnet tool run dotnet-format --folder apps/server --check --verbosity minimal

format-server:
	dotnet tool run dotnet-format --folder apps/server --verbosity minimal

format-clients: format-check-candidate format-check-recruiter

format-fix-candidate:
	cd $(CANDIDATE_CLIENT) && npx prettier --write "**/*.{js,jsx,ts,tsx,json,css,md}"

format-fix-recruiter:
	cd $(RECRUITER_CLIENT) && npx prettier --write "**/*.{js,jsx,ts,tsx,json,css,md}"

format-fix-clients: format-fix-candidate format-fix-recruiter



############## Install dependencies for local setup ############+##
.PHONY: install-all

install-all:
	cd $(CANDIDATE_CLIENT) && npm install
	cd $(RECRUITER_CLIENT) && npm install
	dotnet restore $(SERVER_SOLUTION)

	

############## OpenAPI and type generation ##############
.PHONY: export-openapi generate-ts-types sync-api-contracts

export-openapi:
	dotnet tool run swagger tofile --output ./$(OPENAPI_JSON) ./apps/Server/Server.API/bin/Debug/net8.0/Server.API.dll v1

generate-ts-types:
	cd $(RECRUITER_CLIENT) && npx openapi-typescript ../../$(OPENAPI_JSON) --output src/types/generated/api.d.ts
	cd $(CANDIDATE_CLIENT) && npx openapi-typescript ../../$(OPENAPI_JSON) --output src/types/generated/api.d.ts

sync-api-contracts: build-server export-openapi generate-ts-types