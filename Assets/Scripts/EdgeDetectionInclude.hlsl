#ifndef SOBELOUTLINES_INCLUDED
#define SOBELOUTLINES_INCLUDED

// sample coordinates relative to the starting point
static float2 sobelSamplePoints[9] = {
    float2(-1, 1), float2(0, 1), float2(1,1),
    float2(-1, 0), float2(0, 0), float2(1,0),
    float2(-1, -1), float2(0, -1), float2(1,-1)
};

// weights for the horizontal Sobel sampling
static float sobelXMatrix[9] = {
    1, 0, -1,
    2, 0, -2,
    1, 0, -1
};

// weights for the vertical Sobel sampling
static float sobelYMatrix[9] = {
    1, 2, 1,
    0, 0, 0,
    -1, -2, -1
};

//calculate the edginess of a single point based on the surrounding depth
void SobelEdgeDetection_float(float2 UV, float Thickness, float DistanceThresh, out float Out) {
    float2 sobel = 0;
    [unroll] for (int i=0; i < 9; i++) {
        float depth = SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV + sobelSamplePoints[i] * Thickness);
        if (depth < DistanceThresh) {
            sobel += depth * float2(sobelXMatrix[i], sobelYMatrix[i]);
        } else {
            sobel = 0;
            break;
        }
    }
    // output the final sobel value
    float steepThresh = 0.00003;
    Out = step(steepThresh, length(sobel));
}

#endif