SHELL := bash

# variables
INFRA_MIGRATIONS_DIR = ../../../infra/db/migrations
STARTUP_PROJECT = apps/server/Server.API
INFRA_PROJECT = apps/server/Server.Infrastructure

############# build and start #############

build-server:
	dotnet build apps/server/Server.sln

build-recruiter-client:
	cd apps/recruiter-client && npm run build

build-candidate-client:
	cd apps/candidate-client && npm run build

build-clients: build-recruiter-client build-candidate-client

build: build-server build-clients

start-server:
	dotnet run --project apps/server/Server.API

start-candidate-client:
	cd apps/candidate-client && npm start

start-recruiter-client:
	cd apps/recruiter-client && npm start

start-all: start-server start-candidate-client start-recruiter-client

############## Install dependencies for local setup ##############

install-all:
	cd apps/candidate-client && npm install
	cd apps/recruiter-client && npm install
	dotnet restore apps/server/Server.sln

############## database ##############

add-migration:
ifeq ($(strip $(name)),)
	$(error ❌ Please provide a migration name. Example: make add-migration name=Init)
endif
	dotnet ef migrations add $(name) \
		--project $(INFRA_PROJECT) \
		--startup-project $(STARTUP_PROJECT) \
		--output-dir $(INFRA_MIGRATIONS_DIR)

update-database:
ifeq ($(strip $(name)),)
	dotnet ef database update \
		--project $(INFRA_PROJECT) \
		--startup-project $(STARTUP_PROJECT)
else
	dotnet ef database update $(name) \
		--project $(INFRA_PROJECT) \
		--startup-project $(STARTUP_PROJECT)
endif

seed-database:
	dotnet run --project infra/db/seeding


############## Linting and Formatting ##############

check: lint-clients format-clients format-server
fix: lint-fix-candidate lint-fix-recruiter format-fix-candidate format-fix-recruiter format-server	

lint-candidate:
	cd apps/candidate-client && npx eslint . --ext .js,.jsx,.ts,.tsx

lint-recruiter:
	cd apps/recruiter-client && npx eslint . --ext .js,.jsx,.ts,.tsx

lint-clients: lint-candidate lint-recruiter

lint-fix-candidate:
	cd apps/candidate-client && npx eslint . --ext .js,.jsx,.ts,.tsx --fix

lint-fix-recruiter:
	cd apps/recruiter-client && npx eslint . --ext .js,.jsx,.ts,.tsx --fix

format-check-candidate:
	cd apps/candidate-client && npx prettier --check "**/*.{js,jsx,ts,tsx,json,css,md}"

format-check-recruiter:
	cd apps/recruiter-client && npx prettier --check "**/*.{js,jsx,ts,tsx,json,css,md}"

format-check-server:
	dotnet format --verify-no-changes apps/server/Server.sln

format-server:
	dotnet format apps/server/Server.sln

format-clients: format-check-candidate format-check-recruiter

format-fix-candidate:
	cd apps/candidate-client && npx prettier --write "**/*.{js,jsx,ts,tsx,json,css,md}"

format-fix-recruiter:
	cd apps/recruiter-client && npx prettier --write "**/*.{js,jsx,ts,tsx,json,css,md}"

format-fix-clients: format-fix-candidate format-fix-recruiter