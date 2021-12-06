import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { AuthModule } from './auth/auth.module';
import { ResetModule } from './reset/reset.module';

@Module({
  imports: [AuthModule,
    // TypeOrmModule.forRoot({
    //   type: 'mysql',
    //   host: 'localhost',
    //   port: 3306,
    //   username: 'rootnative',
    //   password: 'rootnative',
    //   database: 'udemynestvueauth',
    //   entities: ['dist/**/*.entity{.ts,.js}'],
    //   synchronize: true
    // }),
    TypeOrmModule.forRoot({
      type: 'sqlite',
      database: 'db.db',
      entities: ['dist/**/*.entity{.ts,.js}'],
      synchronize: true,
    }),
    ResetModule,],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
