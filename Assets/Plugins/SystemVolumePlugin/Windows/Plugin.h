#pragma once

// TODO: Is this correct? This was in the Unity example plugin.
//       https://docs.unity3d.com/uploads/Examples/SimplestPluginExample-4.0.zip
//       https://docs.unity3d.com/Manual/PluginsForDesktop.html
// Visual Studio needs exported functions annotated with this.
#define EXPORT_API __declspec(dllexport)

extern "C"
{
	// Set an optional logging callback / delegate. The rest of these functions
	// call it with details if they fail.
	typedef void(*FuncPtr)(const char *);
	void EXPORT_API SetLoggingCallback(FuncPtr func);

	// Initialize volume handles. Must be called before GetVolume and SetVolume.
	// On failure, return nonzero.
	int EXPORT_API InitializeVolume();

	// Return the default audio device scalar volume between 0.0 and 1.0, inclusive.
	// On failure, return negated error code.
	float EXPORT_API GetVolume();

	// Set the default audio device scalar volume.
	// On failure, return nonzero error code.
	int EXPORT_API SetVolume(const float);
}
