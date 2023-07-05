import { headers } from 'next/headers'
export default function Home() {
  return (
    <main>
      <h1>Howdy!</h1>
      <ul>
        {Object.entries(headers()).map(([key, value]) => (
          <li key={key}>
            <strong>{key}:</strong>
            <ul>
              {Object.entries(value).map(([nestedKey, nestedValue]) => (
                <li key={nestedKey}>
                  <strong>{nestedKey}: </strong><br/>
                  {nestedValue as React.ReactNode}
                </li>
              ))}
            </ul>
          </li>
        ))}
      </ul>
    </main>
  )
}
