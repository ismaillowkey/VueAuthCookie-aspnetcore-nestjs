import { MailerService } from '@nestjs-modules/mailer';
import { BadRequestException, Body, Controller, forwardRef, Inject, NotFoundException, Post } from '@nestjs/common';
import { AuthService } from 'src/auth/auth.service';
import { ResetService } from './reset.service';
import * as bcrypt from 'bcrypt'

@Controller('auth')
export class ResetController {
    constructor(private readonly resetService: ResetService,
        private mailerService: MailerService,
        @Inject(forwardRef(() => AuthService))
        private authService: AuthService) {
    }

    @Post('forgot')
    async forgot(@Body('email') email: string){
        const user = await this.authService.findOneBy({email});
        if (!user)
            throw new NotFoundException('User Not Found');
            
        const token = Math.random().toString().substr(2,12);
        await this.resetService.create({email, token});

        const url = `http://localhost:8080/reset/${token}/${email}`;
        await this.mailerService.sendMail({
            to: email,
            subject: 'Reset your password',
            html: `Click <a href="${url}">here</a> to reset your password!`
        })

        return {
            message: 'Check your email'
        }
    }

    @Post('reset')
    async reset(
        @Body('token') token: string,
        @Body('password') password: string,
        @Body('password_confirm') password_confirm: string
    ){
        if (password !== password_confirm)
            throw new BadRequestException('Password do not match');
        
        const reset = await this.resetService.findOne({token});
        const email = reset.email;
        const user = await this.authService.findOneBy({email});
        if (!user)
            throw new NotFoundException('User Not Found');

        const hashedPassword = await bcrypt.hash(password, 12);
        await this.authService.update(user.id, {password: hashedPassword});

        return {
            message: 'Sucess'
        }
    }

}
