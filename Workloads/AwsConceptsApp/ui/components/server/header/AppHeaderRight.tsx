import React from 'react'
import UserPersonaProvider from '../../client/user/user-persona-provider'
import { ViewToggle } from '@/components/client/layout/ViewToggle'

export const AppHeaderRight = () => {
  return (
    <>
    <UserPersonaProvider/>
    <ViewToggle/>
    </>
  )
}
