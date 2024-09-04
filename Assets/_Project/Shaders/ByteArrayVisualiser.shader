Shader "ByteArrayVisualiser"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            static const uint kScreenYLookup[] =
            {
                0,
                8,
                16,
                24,
                32,
                40,
                48,
                56,
                1,
                9,
                17,
                25,
                33,
                41,
                49,
                57,
                2,
                10,
                18,
                26,
                34,
                42,
                50,
                58,
                3,
                11,
                19,
                27,
                35,
                43,
                51,
                59,
                4,
                12,
                20,
                28,
                36,
                44,
                52,
                60,
                5,
                13,
                21,
                29,
                37,
                45,
                53,
                61,
                6,
                14,
                22,
                30,
                38,
                46,
                54,
                62,
                7,
                15,
                23,
                31,
                39,
                47,
                55,
                63,
                64,
                72,
                80,
                88,
                96,
                104,
                112,
                120,
                65,
                73,
                81,
                89,
                97,
                105,
                113,
                121,
                66,
                74,
                82,
                90,
                98,
                106,
                114,
                122,
                67,
                75,
                83,
                91,
                99,
                107,
                115,
                123,
                68,
                76,
                84,
                92,
                100,
                108,
                116,
                124,
                69,
                77,
                85,
                93,
                101,
                109,
                117,
                125,
                70,
                78,
                86,
                94,
                102,
                110,
                118,
                126,
                71,
                79,
                87,
                95,
                103,
                111,
                119,
                127,
                128,
                136,
                144,
                152,
                160,
                168,
                176,
                184,
                129,
                137,
                145,
                153,
                161,
                169,
                177,
                185,
                130,
                138,
                146,
                154,
                162,
                170,
                178,
                186,
                131,
                139,
                147,
                155,
                163,
                171,
                179,
                187,
                132,
                140,
                148,
                156,
                164,
                172,
                180,
                188,
                133,
                141,
                149,
                157,
                165,
                173,
                181,
                189,
                134,
                142,
                150,
                158,
                166,
                174,
                182,
                190,
                135,
                143,
                151,
                159,
                167,
                175,
                183,
                191,
            };

            static const fixed3 kAttributeColors[8] =
            {
                { 0, 0, 0 }, // 0 black
                { 0, 0, 1 }, // 1 blue
                { 1, 0, 0 }, // 2 red
                { 1, 0, 1 }, // 3 magenta
                { 0, 1, 0 }, // 4 green
                { 0, 1, 1 }, // 5 cyan
                { 1, 1, 0 }, // 6 yellow
                { 1, 1, 1 }, // 7 white
            };

            static const int kScreenWidthPixels = 256;
            static const int kScreenHeightPixels = 192;

            static const int kScreenWidthCharacters = kScreenWidthPixels / 8;
            static const int kScreenHeightCharacters = kScreenHeightPixels / 8;

            static const int kScreenPixelDataLength = kScreenWidthCharacters * kScreenHeightPixels;

            static const int kScreenAttributeDataLength = kScreenWidthCharacters * kScreenHeightCharacters;

            static const int kScreenTotalDataLength = kScreenPixelDataLength + kScreenAttributeDataLength;

            // memory
            static const int kMemoryRomLength = 16384;
            static const int kMemoryRamLength = 49152;
            static const int kMemoryTotalLength = kMemoryRomLength + kMemoryRamLength;

            static const int kMemoryScreenPixelsStart = kMemoryRomLength;
            static const int kMemoryScreenAttributesStart = kMemoryScreenPixelsStart + kScreenPixelDataLength;



            sampler2D _MainTex;
            float4 _MainTex_ST;

            // Declare the compute buffer
            StructuredBuffer<uint> _ByteBuffer;


            uint ReadMemoryByte(int address)
            {
                // Determine which uint contains the byte and the byte's position within that uint
                uint targetIndex = address / 4;
                uint byteOffset = address % 4;

                // Extract the byte from the corresponding position
                uint targetValue = _ByteBuffer[targetIndex];

                // Shift the bytes in the uint so the desired byte is in the lowest position
                uint shiftAmount = byteOffset * 8; // 8 bits per byte
                uint result = ((targetValue >> shiftAmount) & 0xFF);

                return result;
            }


            uint GetAttributeByte(v2f i)
            {
                float2 characterCoord = i.uv * float2(kScreenWidthCharacters, kScreenHeightCharacters);

                uint characterX = floor(characterCoord.x);
                uint characterY = kScreenHeightCharacters - 1 - floor(characterCoord.y);

                uint byteIndex = (characterY * kScreenWidthCharacters) + characterX;

                uint byteValue = ReadMemoryByte(kMemoryScreenAttributesStart + byteIndex);

                return byteValue;
            }

            bool IsBitSet(v2f i)
            {
                // Calculate the byte and bit index based on UV coordinates
                float2 pixelCoord = i.uv * float2(kScreenWidthPixels, kScreenHeightPixels);

                uint screenX = floor(pixelCoord.x);
                uint screenYRaw = kScreenHeightPixels - 1 - floor(pixelCoord.y);
                uint screenYDecoded = kScreenYLookup[screenYRaw];

                uint byteIndex = (screenYDecoded * kScreenWidthCharacters) + (screenX / 8);

                uint bitIndex = 7 - (screenX % 8);

                // Default color (can be used for areas outside the byte array)
                fixed4 col = fixed4(0.5, 0.5, 0.5, 1); // Gray

                // Ensure we're within the array bounds
                if (byteIndex < kScreenPixelDataLength)
                {
                    // Check if the current bit is set
                    uint byteValue = ReadMemoryByte(kMemoryScreenPixelsStart + byteIndex);

                    bool isBitSet = (byteValue & (1 << bitIndex)) != 0;
                    
                    return isBitSet;
                }

                return false;
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                bool isBitSet = IsBitSet(i);

                uint attributeByte = GetAttributeByte(i);
                uint inkValue = attributeByte & 7;
                uint paperValue = (attributeByte >> 3) & 7;
                uint brightValue = (attributeByte >> 6) & 1;
                uint flashValue = (attributeByte >> 7) & 1;

                fixed4 inkColor = fixed4(kAttributeColors[inkValue], 1);
                fixed4 paperColor = fixed4(kAttributeColors[paperValue], 1);

                if (!brightValue)
                {
                    inkColor *= 0.8;
                    paperColor *= 0.8;
                }

                // seems very slightly out (compared to Spectaculator), but calced from:
                //   The Spectrum's 'FLASH' effect is also produced by the ULA: Every 16 frames, the 
                //   ink and paper of all flashing bytes is swapped; ie a normal to inverted to normal 
                //   cycle takes 32 frames, which is (good as) 0.64 seconds.
                uint timeInt = floor(_Time.y * 3.125);
                if (flashValue && timeInt % 2 == 0)
                {
                    fixed4 tempInkColor = inkColor;
                    inkColor = paperColor;
                    paperColor = tempInkColor;
                }

                fixed4 col = isBitSet ? inkColor : paperColor;

                return col;
            }
            ENDCG
        }
    }
}
