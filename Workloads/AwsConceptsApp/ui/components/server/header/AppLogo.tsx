import React from 'react'
import Image from 'next/image'

const Applogo = () => {
  return (
    <>
    <Image
          src="/AwsConceptLogoRaw-nbg.png"
          alt="Aws concepts Logo"
          className="dark:invert"
          width={50}
          height={50}
          priority />
          <span>Aws Concepts</span>
    </>
  )
}

export default Applogo