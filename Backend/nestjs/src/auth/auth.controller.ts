import { BadRequestException, Body, ClassSerializerInterceptor, Controller, Get, Post, Req, Res, UseInterceptors } from '@nestjs/common';
import { AuthService } from './auth.service';
import { AddUserDTO } from './DTO/addUser.dto';
import * as bcrypt from 'bcrypt';
import { JwtService } from '@nestjs/jwt';
import { Request, Response } from 'express';
import { User } from './models/user.entity';
import { AuthInterceptor } from './auth.interceptor';

@Controller('auth')
export class AuthController {
    constructor(
        private readonly authService: AuthService,
        private jwtService: JwtService
    ){}

    @UseInterceptors(ClassSerializerInterceptor)
    @Post('register')
    async register(@Body() body: AddUserDTO)
    {
        const email = body.email;
        const user = await this.authService.findOneBy({email});
        if (user){
            throw new BadRequestException('Email already exist');
        }

        if (body.password !== body.password_confirm){
            throw new BadRequestException('Password do not match')
        }

        body.password = await bcrypt.hash(body.password, 12);
        //body.password_confirm = await bcrypt.hash(body.password, 12);
        await this.authService.create(body);

        return {
            "statusCode": 201,
            message: 'success create account',
            data: {
                first_name : body.first_name,
                last_name : body.last_name,
                email : body.email
             }
        }
    }

    @UseInterceptors(ClassSerializerInterceptor)
    @Post('login')
    async login(
        @Body('email') email: string,
        @Body('password') password: string,
        @Res({passthrough: true}) response: Response
    ){
        const user: User = await this.authService.findOneBy({email});
        if (!user){
            throw new BadRequestException('Email does not exist');
        }

        if (!await bcrypt.compare(password, user.password)){
            throw new BadRequestException('Invalid credentials')
        }

        const jwt = await this.jwtService.signAsync({id: user.id});
        response.cookie('jwt', jwt, {httpOnly: true})
        return {
            "statusCode": 200,
            message: 'User has logged in',
            data: {
                first_name : user.first_name,
                last_name : user.last_name,
                email : user.email
            }
        };
    }

    @UseInterceptors(ClassSerializerInterceptor, AuthInterceptor)
    @Get('user')
    async user(@Req() request: Request){
        const cookie = request.cookies['jwt'];
        const data = await this.jwtService.verifyAsync(cookie);
        const user = await this.authService.findOneBy({id: data['id']});
        return {
            "statusCode": 200,
            message: '',
            data: {
                first_name : user.first_name,
                last_name : user.last_name,
                email : user.email
            }
        };
    }

    @UseInterceptors(ClassSerializerInterceptor)
    @Post('logout')
    async logout(
        @Res({passthrough: true}) response: Response
    ){
        response.clearCookie('jwt');
        return {
            message: 'You are logged out'
        }
    }
}
