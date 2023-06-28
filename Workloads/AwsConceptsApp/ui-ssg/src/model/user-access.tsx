  export interface UserProfile {
    email:string;
    nickName:string;
    givenName:string;
    surname:string;
  }

  export const defaultUserProfile:UserProfile = {
    email: "",
    nickName: "",
    givenName: "",
    surname: ""
  }

  export const populateUserProfile = (user) => {
    let userProfile = defaultUserProfile;
    if (user?.attributes) {
      userProfile.email = user.attributes.email || userProfile.email;
      userProfile.nickName = user.attributes.nickname || userProfile.nickName;
      userProfile.givenName = user.attributes.given_name || userProfile.givenName;
      userProfile.surname = user.attributes.family_name || userProfile.surname;
    }
    return userProfile;
  }