package com.zenika.elections.producer;

import java.io.IOException;

import com.rabbitmq.client.AMQP;
import com.rabbitmq.client.Channel;
import com.rabbitmq.client.Connection;
import com.rabbitmq.client.ConnectionFactory;

/**
 * 
 */
public class Producer {

	private final static String MESSAGE = "<Dpt> <CodReg>82</CodReg> <CodReg3Car>082</CodReg3Car> <CodDpt>__DEPARTEMENT__</CodDpt>"
			+ "<CodDpt3Car>001</CodDpt3Car> <CodMinDpt>01</CodMinDpt> <LibDpt>AIN</LibDpt>"
			+ "<DateClotureDpt>19-07-2011</DateClotureDpt> <HeureClotureDpt>14:59:04</HeureClotureDpt>"
			+ "<Clos>CLOS</Clos> <Candidat> <Nom>SARKOZY</Nom> <Prenom>Nicolas</Prenom>"
			+ "<Civilite>M.</Civilite> <Voix>185165</Voix> <RapVoixExp>60,54</RapVoixExp>"
			+ "<NumDepCand>12</NumDepCand> <NumPanneauCand>1</NumPanneauCand> </Candidat> </Dpt>";

	public static void main(String[] args) throws IOException, InterruptedException {
		ConnectionFactory factory = new ConnectionFactory();
		factory.setUsername("guest");
		factory.setPassword("guest");
		factory.setVirtualHost("/");
		factory.setHost("127.0.0.1");
		// factory.setPort();

		Connection connection = factory.newConnection();
		Channel channel = connection.createChannel();

		// channel.exchangeDeclare("elections", "fanout");

		AMQP.BasicProperties.Builder builder = new AMQP.BasicProperties.Builder();
		AMQP.BasicProperties properties = builder.contentType("application/xml").deliveryMode(2).build();

		for (int i = 10; i < 96; i++) {
			String currResult = MESSAGE.replace("__DEPARTEMENT__", "" + i);
			channel.basicPublish("elections", "key", properties, currResult.getBytes());
			System.out.println("Published result for departement " + i + '.');

			Thread.sleep(3000);
		}

		channel.close();
		connection.close();
	}

}
