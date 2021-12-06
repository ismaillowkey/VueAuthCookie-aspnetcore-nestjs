import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { AddUserDTO } from './DTO/addUser.dto';
import { User } from './models/user.entity';

@Injectable()
export class AuthService {
    constructor(@InjectRepository(User) private readonly userRepository: Repository<User>
    ) { }

    async create(obj: AddUserDTO): Promise<User>{
        return await this.userRepository.save(obj);
    }

    async findOneBy(condition): Promise<User> {
        return await this.userRepository.findOne(condition);
    }

    async update(id: number, data): Promise<any>{
        return await this.userRepository.update(id, data);
    }
}
