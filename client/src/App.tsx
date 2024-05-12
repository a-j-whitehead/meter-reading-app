import { ChangeEvent, useState } from 'react';
import './App.css';
import axios from 'axios';

function App() {
    const [file, setFile] = useState<File>();
    const [responseMessage, setResponseMessage] = useState<string>()

    const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target.files) {
          setFile(e.target.files[0]);
        }
      };

    const handleApiResponse = (response: string, error: boolean) => {
      setResponseMessage(error ? `Error: ${ response }` : response)
    }
    
    const handleUploadClick = () => {
        if (!file) {
          return;
        }

        const formData = new FormData();
        formData.append("meter_readings", file)

        axios.post(
          'https://localhost:44308/submit-meter-readings',
          formData,
          {
            headers: {
              'Content-type': "multipart/form-data",
              'Access-Control-Allow-Origin': '*',
            },
          }
        )
        .then((res) => handleApiResponse(res.data, false))
        .catch((err) => handleApiResponse(err, true));
      };
    
    return (
        <div>
            <h1>Upload Meter Reading</h1>
            <p>Upload a valid csv file below, with "AccountId", "MeterReadingDateTime" and "MeterReadValue" headers</p>
            <br />
            <input type="file" onChange={handleFileChange} />
            <br />
            <br />
            <button onClick={handleUploadClick}>Submit</button>
            <br />
            <div>{responseMessage}</div>
        </div>
    );
}

export default App;
