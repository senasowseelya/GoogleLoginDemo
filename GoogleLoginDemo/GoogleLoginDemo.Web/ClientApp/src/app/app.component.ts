import { Component } from '@angular/core';
import { GoogleLoginProvider, SocialAuthService, SocialUser } from 'angularx-social-login';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
  user! : SocialUser | null;
  constructor(private authService:SocialAuthService){
    this.user = null;
    this.authService.authState.subscribe((user :SocialUser)=>{
        this.user = user;
        console.log(this.user);
    })
  }

  signInWithGoogle(){
    this.authService.signIn(GoogleLoginProvider.PROVIDER_ID).then(x=> console.log(x));
  }

  signOut(){
    this.authService.signOut();
  }
}
