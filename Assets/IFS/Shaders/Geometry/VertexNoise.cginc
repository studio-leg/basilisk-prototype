#ifndef VertexNoise_INCLUDED
#define VertexNoise_INCLUDED

#include "../../Common/Shaders/Random.cginc"
#include "../../Common/Shaders/Noise/SimplexNoise3D.cginc"

float4 _NoiseScale;
float4 _Intensity;
float4 _NormalNoiseScale;
float _NoiseTime;
float _NormalVertOffset;
float _NormalLerp;
float _WorldSpace;

// Vertex noise / position helpers

float getNoise(float3 p, float k)
{
    float n1 = snoise(p);
    return n1;
}

float4 getNextVertPosition(float4 vert, float4 normal)
{
    float4 worldSpaceVertex = mul(unity_ObjectToWorld, vert);
    float noise = getNoise(worldSpaceVertex.xyz * _NoiseScale.xyz + _NoiseTime, 1.0);
    float4 localNoise = vert + (normal * noise * _Intensity);
    float4 worldNoise = vert + (noise * _Intensity);
    return lerp(localNoise, worldNoise, _WorldSpace);
}


		// Vertex displacement and normal recalculation
void applyVertexNoise(inout appdata_full v)
{
    float4 worldSpaceVertex = v.vertex;
    float4 normal = float4(v.normal, 1.0);
    float4 vertPosition = getNextVertPosition(worldSpaceVertex, normal);

			// calculate the bitangent (sometimes called binormal) from the cross product of the normal and the tangent
    float4 bitangent = float4(cross(v.normal, v.tangent), 0);

			// Offset of vert position to calculate the new normal
			// Larger values produce smoother results but with less surface detail
    float vertOffset = _NormalVertOffset;

			// Calculate 2 normals based on points in opposite directions from the Vertex
			// Then lerp these normals for a smoother end result

			// Normal 1
    float4 v1 = getNextVertPosition(worldSpaceVertex + v.tangent * vertOffset, normal);
    float4 v2 = getNextVertPosition(worldSpaceVertex + bitangent * vertOffset, normal);
			// Create new tangents and bitangents based on the deformed positions
    float4 newTangent1 = v1 - vertPosition;
    float4 newBitangent1 = v2 - vertPosition;
			// recalculate the normal based on the new tangent & bitangent
    float3 normal1 = cross(newTangent1, newBitangent1);

			// Normal 2, in the opposite direction
    float4 v3 = getNextVertPosition(worldSpaceVertex + v.tangent * -vertOffset, normal);
    float4 v4 = getNextVertPosition(worldSpaceVertex + bitangent * -vertOffset, normal);
			// Create new tangents and bitangents based on the deformed positions
    float4 newTangent2 = v3 - vertPosition;
    float4 newBitangent2 = v4 - vertPosition;
			// recalculate the normal based on the new tangent & bitangent
    float3 normal2 = cross(newTangent2, newBitangent2);

			// Now lerp between the 2 normals
    v.normal = lerp(normal1, normal2, _NormalLerp);

			// takes the new modified position of the vert in world space and then puts it back in local space?
    v.vertex = vertPosition;
}

#endif // VertexNoise_INCLUDED
