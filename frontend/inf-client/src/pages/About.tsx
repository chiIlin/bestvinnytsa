import { Linkedin, Instagram } from 'lucide-react';

const team = [
	{
		name: 'Дмитро',
		role: 'Front-end розробник',
		linkedin:
			'https://www.linkedin.com/in/%D0%B4%D0%BC%D0%B8%D1%82%D1%80%D0%BE-%D0%B4%D0%B8%D1%88%D0%BB%D0%B5%D0%B2%D0%B5%D0%BD%D0%BA%D0%BE-72143a26a/',
		instagram: 'https://www.instagram.com/sodd7m/',
		avatar:
			'https://media.licdn.com/dms/image/v2/D4D03AQFaDit3LE5qKg/profile-displayphoto-shrink_800_800/B4DZWJfKosHkAc-/0/1741768404207?e=1753920000&v=beta&t=kg3giRF7ETWdJZh5hOqcz6KYyZ74diEItfRHvL5ONiU', // посилання на фото
		description:
			'Відповідає за інтерфейс, дизайн та користувацький досвід платформи.',
	},
	{
		name: 'Давид',
		role: 'Front-end розробник',
		linkedin: 'https://www.linkedin.com/in/david-kachur-96a874327/',
		instagram: '#',
		avatar:
			'https://media.licdn.com/dms/image/v2/D5603AQFqgsuGAOme8g/profile-displayphoto-shrink_200_200/B56ZVS_isTHsAY-/0/1740854145331?e=1753920000&v=beta&t=E75t2zqYm5bx8Wq1_IvU9vnokA7oVwgKWqU5ITkoNfc', // посилання на фото
		description: 'Розробляє сучасний та адаптивний фронтенд, інтегрує нові фічі.',
	},
	{
		name: 'Руслан',
		role: 'Back-end розробник',
		linkedin: 'https://www.linkedin.com/in/ruslan-shevchyk-749835320/',
		instagram: '#',
		avatar:
			'https://media.licdn.com/dms/image/v2/D5603AQEV0WB2I8L_LA/profile-displayphoto-shrink_200_200/B56ZQPDquHGsAY-/0/1735419408516?e=1753920000&v=beta&t=dxEyoEz88BDonudVRx04ABwOwbSvZnj5bjD1vL0rt3k', // посилання на фото
		description: 'Забезпечує надійну роботу серверної частини та API.',
	},
	{
		name: 'Олексій',
		role: 'Back-end розробник',
		linkedin: 'https://www.linkedin.com/in/%D0%BE%D0%BB%D0%B5%D0%BA%D1%81%D1%96%D0%B9-%D0%BC%D0%B0%D0%BB%D0%B0%D0%BD%D1%96%D0%B9-b9a579306/',
		instagram: '#',
		avatar:
			'https://media.licdn.com/dms/image/v2/D4D03AQHDwwBHzTcn7Q/profile-displayphoto-shrink_200_200/B4DZY5xexgH4Ac-/0/1744725995303?e=1753920000&v=beta&t=Q3sQfkHnK1b6x12JVSZTCb1mWYJYKhMDG5htkMh_ChA', // посилання на фото
		description:
			'Відповідає за базу даних, безпеку та масштабованість платформи.',
	},
];

const About = () => (
	<div className="min-h-screen flex flex-col">

		<section className="flex-1 py-16 px-4">
			<div className="container mx-auto max-w-3xl">
				<div className="bg-white/60 backdrop-blur-sm rounded-2xl border border-ua-pink-light p-8 md:p-12">
					<div className="text-center mb-12">
						<h1 className="text-4xl font-bold mb-4 gradient-text">Про нас</h1>
						<p className="text-xl text-gray-600">
							Ми — команда розробників зі Львова, яка створила UA Influencer
							Connect, щоб допомогти українським інфлюенсерам та брендам знаходити
							одне одного та ефективно співпрацювати.
						</p>
					</div>
					<div className="grid md:grid-cols-2 gap-8">
						{team.map((member) => (
							<div
								key={member.name}
								className="bg-white/80 rounded-xl p-6 border border-ua-blue-light flex flex-col items-center text-center h-full"
								style={{ minHeight: 320 }}
							>
								<div className="w-20 h-20 bg-gradient-to-r from-ua-pink to-ua-blue rounded-full flex items-center justify-center mb-4 overflow-hidden">
									<img
										src={member.avatar}
										alt={member.name}
										className="w-full h-full object-cover"
									/>
								</div>
								<h2 className="text-xl font-semibold mb-1">{member.name}</h2>
								<p className="text-ua-blue font-medium mb-2">{member.role}</p>
								<div className="flex-1 flex flex-col justify-between w-full">
									<p className="text-gray-700 mb-3">{member.description}</p>
									<div className="flex justify-center gap-4 mt-auto">
										<a
											href={member.linkedin}
											target="_blank"
											rel="noopener noreferrer"
											className="inline-flex items-center text-ua-blue hover:text-ua-pink transition-colors"
										>
											<Linkedin className="h-5 w-5 mr-1" />
											LinkedIn
										</a>
										<a
											href={member.instagram}
											target="_blank"
											rel="noopener noreferrer"
											className="inline-flex items-center text-ua-blue hover:text-ua-pink transition-colors"
										>
											<Instagram className="h-5 w-5 mr-1" />
											Instagram
										</a>
									</div>
								</div>
							</div>
						))}
					</div>
				</div>
			</div>
		</section>

	</div>
);

export default About;