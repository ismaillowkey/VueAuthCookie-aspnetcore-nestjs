import { Column, Entity, PrimaryGeneratedColumn } from "typeorm";

@Entity('password_reset')
export class ResetEntity {
    @PrimaryGeneratedColumn()
    id: string;

    @Column()
    email: string;

    @Column()
    token: string;
}