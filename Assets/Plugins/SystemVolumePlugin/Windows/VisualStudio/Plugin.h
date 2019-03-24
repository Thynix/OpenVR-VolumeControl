#pragma once

// TODO: really? this was in the unity sample
#define EXPORT_API __declspec(dllexport) // Visual Studio needs exported functions annotated with this

extern "C"
{
	// Initialize the 
	int EXPORT_API InitializeVolume();

	// Return the default audio device scalar volume setting between 0.0 and 1.0, inclusive.
	// On failure, return negated error code.
	float EXPORT_API GetVolume();

	int EXPORT_API SetVolume(const float);
}
