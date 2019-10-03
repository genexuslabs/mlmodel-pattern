# Machine Learning Model Pattern
This pattern creates the necessary GeneXus objects to be used with the [GeneXusAI module](https://wiki.genexus.com/commwiki/servlet/wiki?40315) to train a regression or classification machine learning model.

## Building

Building requires the following environment variables to be set (or passed as properties in the MSBuild invokation):
- GX_PROGRAM_DIR - pointing to your local GX installation
- GX_SDK_DIR - pointing to your local GX SDK installation

To build (and deploy) the pattern run the following command:

```
msbuild MLModel.msbuild
```
