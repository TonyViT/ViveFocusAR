//MIT License

//Copyright(c) 2018-2019 Antony Vitillo(a.k.a. "Skarredghost")

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMR : MonoBehaviour
{
    /// <summary>
    /// True to have only one instance of the Wave Mixed Reality prefab, false otherwise
    /// </summary>
    [SerializeField]
    private bool m_isSingleton;

    /// <summary>
    /// Reference to the singleton instance
    /// </summary>
    private static WaveMR Instance; 

    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        if (m_isSingleton)
        {
            //singleton pattern

            //Check if instance already exists
            if (Instance == null)
            {

                //if not, set instance to this
                Instance = this;

                DontDestroyOnLoad(gameObject);

                transform.Find("AR_background").gameObject.SetActive(true);
            }

            //If instance already exists and it's not this:
            else if (Instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
        }
        else
            transform.Find("AR_background").gameObject.SetActive(true);
    }
    
}
