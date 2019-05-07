PACKAGE_VERSION=0.1.0#

clean:
	rm -r ../artefacts || true
	rm -r ../*/*/obj || true
	rm -r ../*/*/bin || true

	mkdir -p ../artefacts/nuget

build:
	dotnet restore ../

# SpecFlowPlugin must be built before the tests
	dotnet build -c Release --no-restore ../src/XunitRetry.SpecFlowPlugin
	dotnet build -c Release --no-restore ../test/UnitTests

unit-tests-run:
	cd ../test/UnitTests && \
		dotnet test --no-build -c Release --logger:trx\;logfilename=../../../artefacts/testResults/UnitTests.trx

nuget-create:
	dotnet pack ../src/XunitRetry \
		/p:Version=$(PACKAGE_VERSION) \
		--no-build \
		-c Release \
		-o ../../artefacts/nuget

	dotnet pack ../src/XunitRetry.SpecFlowPlugin \
		/p:Version=$(PACKAGE_VERSION) \
		--no-build \
		-c Release \
		-o ../../artefacts/nuget

# Args:
#	- apiKey
nuget-publish:
	dotnet push --source https://api.nuget.org/v3/index.json --api-key $(apiKey) ../artefacts/nuget/\*.nupkg